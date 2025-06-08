import { randomIntBetween } from 'https://jslib.k6.io/k6-utils/1.2.0/index.js';
import { check, sleep } from 'k6';
import { SharedArray } from 'k6/data';
import http from 'k6/http';
import { Counter } from 'k6/metrics';

export const options = {
  summaryTrendStats: ['avg', 'min', 'med', 'max', 'p(90)', 'p(95)', 'p(99)', 'p(99.99)', 'count'],

  // https://grafana.com/docs/k6/latest/using-k6/k6-options/reference/#discard-response-bodies
  // https://grafana.com/docs/k6/latest/javascript-api/k6-http/params/#example-of-overriding-discardresponsebodies
  // discardResponseBodies: true,

  // https://grafana.com/docs/k6/latest/using-k6/k6-options/reference/#no-connection-reuse
  // noConnectionReuse: true,

  // https://grafana.com/docs/k6/latest/using-k6/k6-options/reference/#no-vu-connection-reuse
  // desativado para evitar exaustão das portas tcp do host (erro: dial tcp 172.18.0.4:8080: connect: cannot assign requested address)
  // noVUConnectionReuse: false, // reutiliza conexões dentro de uma iteração

  // https://grafana.com/docs/k6/latest/using-k6/k6-options/reference/#throw
  throw: true,

  // stages - shortcut option for a single scenario with a ramping VUs executor
  // https://grafana.com/docs/k6/latest/using-k6/k6-options/reference/#stages

  // https://grafana.com/docs/k6/latest/using-k6/k6-options/reference/#scenarios
  scenarios: {
    breakpoint: {
      executor: 'ramping-arrival-rate',
      startRate: 0,
      timeUnit: '1s',
      preAllocatedVUs: 1000,
      stages: [{ duration: '20m', target: 200 }],
    },
  },

  // https://grafana.com/docs/k6/latest/using-k6/k6-options/reference/#thresholds
  thresholds: {
    'http_req_duration{name:top-assets}': [
      {
        threshold: 'p(99)<2000',
        abortOnFail: true,
        delayAbortEval: '30s',
      },
    ],
    'http_req_duration{name:top-asset-quotation-histories}': [
      {
        threshold: 'p(99)<2000',
        abortOnFail: true,
        delayAbortEval: '30s',
      },
    ],
    'http_req_duration{name:random-quotation-histories}': [
      {
        threshold: 'p(99)<2000',
        abortOnFail: true,
        delayAbortEval: '30s',
      },
    ],
  },
};

const BASE_API_URL = 'http://engs-tcc-webapi:8080';
const NUMBER_OF_TOP_ASSETS = 50;
const DEFAULT_REQUESTS_TIMEOUT = '10s';

function buildTopAssetsUrl(n) {
  return `${BASE_API_URL}/api/assets?n=${n}`;
}

function buildQuotationHistoriesUrl(paperNegotiationCode) {
  return `${BASE_API_URL}/api/quotation-histories?paperNegotiationCode=${paperNegotiationCode}`;
}

const topAssetsUrl = buildTopAssetsUrl(NUMBER_OF_TOP_ASSETS);

const topAssetsCounter = new Counter('top_assets_counter');
const randomQuotationHistoriesCounter = new Counter('random_quotation_histories_counter');

const paperNegotiationCodesAvailable = new SharedArray(
  'paper negotiation codes available',
  function () {
    return JSON.parse(open('./assets_from_regular_and_odd_lot_markets.json')).paperNegotiationCodes;
  }
);

export default function () {
  const doTopAssetsFlow = Math.random() < 0.5;

  if (doTopAssetsFlow) {
    topAssetsCounter.add(1);

    const topAssetsResponse = http.get(topAssetsUrl, {
      tags: { name: 'top-assets' },
      timeout: DEFAULT_REQUESTS_TIMEOUT,
    });

    check(topAssetsResponse, {
      'top-assets: response is 200': (x) => x.status === 200,
    });

    if (topAssetsResponse.status !== 200) return;

    const topAssetsJsonResponse = topAssetsResponse.json();

    check(topAssetsJsonResponse, {
      'top-assets: response is JSON': (x) => x !== undefined,
    });

    if (topAssetsJsonResponse === undefined) return;

    const topAssets = topAssetsJsonResponse.assets;

    check(topAssets, {
      [`top-assets: response has ${NUMBER_OF_TOP_ASSETS} assets`]: (x) =>
        x.length === NUMBER_OF_TOP_ASSETS,
    });

    if (topAssets.length !== NUMBER_OF_TOP_ASSETS) return;

    const topAssetIndexToGetQuotationHistories = randomIntBetween(0, topAssets.length - 1);
    const paperNegotiationCodeToGetQuotationHistories =
      topAssets[topAssetIndexToGetQuotationHistories].paperNegotiationCode;
    const paperNegotiationCodeQuotationHistoriesUrl = buildQuotationHistoriesUrl(
      paperNegotiationCodeToGetQuotationHistories
    );

    const secondsToWaitToGetQuotationHistories = 2 + 8 * Math.random();
    sleep(secondsToWaitToGetQuotationHistories);

    const paperNegotiationCodeQuotationHistoriesResponse = http.get(
      paperNegotiationCodeQuotationHistoriesUrl,
      {
        tags: { name: 'top-asset-quotation-histories' },
        responseType: 'none',
        timeout: DEFAULT_REQUESTS_TIMEOUT,
      }
    );

    check(paperNegotiationCodeQuotationHistoriesResponse, {
      'top-asset-quotation-histories: response is 200': (x) => x.status === 200,
    });
  } else {
    randomQuotationHistoriesCounter.add(1);

    const paperNegotiationCodeIndexToGetQuotationHistories = randomIntBetween(
      0,
      paperNegotiationCodesAvailable.length - 1
    );
    const paperNegotiationCodeToGetQuotationHistories =
      paperNegotiationCodesAvailable[paperNegotiationCodeIndexToGetQuotationHistories];

    const randomPaperNegotiationCodeUrl = buildQuotationHistoriesUrl(
      paperNegotiationCodeToGetQuotationHistories
    );
    const randomPaperNegotiationCodeResponse = http.get(randomPaperNegotiationCodeUrl, {
      tags: { name: 'random-quotation-histories' },
      responseType: 'none',
      timeout: DEFAULT_REQUESTS_TIMEOUT,
    });

    check(randomPaperNegotiationCodeResponse, {
      'random-quotation-histories: response is 200': (x) => x.status === 200,
    });
  }
}

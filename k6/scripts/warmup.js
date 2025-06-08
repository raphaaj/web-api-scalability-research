import { sleep } from 'k6';
import http from 'k6/http';

export const options = {
  summaryTrendStats: ['avg', 'min', 'med', 'max', 'p(90)', 'p(95)', 'p(99)', 'p(99.99)', 'count'],

  // https://grafana.com/docs/k6/latest/using-k6/k6-options/reference/#discard-response-bodies
  // https://grafana.com/docs/k6/latest/javascript-api/k6-http/params/#example-of-overriding-discardresponsebodies
  discardResponseBodies: true,

  // https://grafana.com/docs/k6/latest/using-k6/k6-options/reference/#no-connection-reuse
  // noConnectionReuse: true,

  // https://grafana.com/docs/k6/latest/using-k6/k6-options/reference/#no-vu-connection-reuse
  // noVUConnectionReuse: false, // reutiliza conexões dentro de uma iteração

  // https://grafana.com/docs/k6/latest/using-k6/k6-options/reference/#throw
  throw: true,

  // https://grafana.com/docs/k6/latest/testing-guides/test-types/smoke-testing/
  vus: 80,
  duration: '1m',
};

const BASE_API_URL = 'http://engs-tcc-webapi:8080';

function buildTopAssetsUrl(n) {
  return `${BASE_API_URL}/api/assets?n=${n}`;
}

function buildQuotationHistoriesUrl(paperNegotiationCode) {
  return `${BASE_API_URL}/api/quotation-histories?paperNegotiationCode=${paperNegotiationCode}`;
}

export default function () {
  const rnd = Math.random();

  if (rnd < 0.5) {
    const topAssetsUrl = buildTopAssetsUrl(100);
    http.get(topAssetsUrl, {
      tags: { name: 'top-assets' },
    });
  } else {
    const quotationHistoriesUrl = buildQuotationHistoriesUrl('VALE3');
    http.get(quotationHistoriesUrl, {
      tags: { name: 'quotation-histories' },
    });
  }

  sleep(1);
}

CREATE TABLE quotation_histories (
    id SERIAL PRIMARY KEY,
    negotiation_date DATE,
    bdi_code CHAR(2),
    paper_negotiation_code VARCHAR(12),
    market_type INTEGER,
    company_abbreviated_name VARCHAR(12),
    paper_specification VARCHAR(10),
    forward_market_term_in_days CHAR(3),
    reference_currency VARCHAR(4),
    opening_floor_price NUMERIC(13,2),
    highest_floor_price NUMERIC(13,2),
    lowest_floor_price NUMERIC(13,2),
    average_floor_price NUMERIC(13,2),
    last_negotiated_price NUMERIC(13,2),
    best_purchase_offer_price NUMERIC(13,2),
    best_sales_offer_price NUMERIC(13,2),
    number_of_trades_conducted BIGINT,
    total_quantity_of_titles_traded BIGINT,
    total_volume_of_titles_negotiated NUMERIC(18,2),
    strike_price_for_options_market NUMERIC(13,2),
    correction_indicator_for_strike_price SMALLINT,
    maturity_date_for_options_market DATE,
    paper_quotation_factor INTEGER,
    strike_price_usd_points NUMERIC(13,6),
    isin_code CHAR(12),
    paper_distribution_number INTEGER
);

CREATE INDEX idx_quotation_histories_code_date_id
ON quotation_histories (paper_negotiation_code, negotiation_date, id);



CREATE MATERIALIZED VIEW assets_aggregation AS
SELECT
  q.paper_negotiation_code,
  SUM(q.total_volume_of_titles_negotiated) AS total_volume_of_titles_negotiated
FROM quotation_histories AS q
WHERE q.paper_negotiation_code IS NOT NULL
AND q.total_volume_of_titles_negotiated IS NOT NULL
GROUP BY q.paper_negotiation_code;

CREATE INDEX idx_assets_aggregation_volume_desc_code
ON assets_aggregation (total_volume_of_titles_negotiated DESC, paper_negotiation_code);



CREATE USER app_b3_quotation_histories_webapi WITH PASSWORD '****';
GRANT SELECT ON quotation_histories TO app_b3_quotation_histories_webapi;
GRANT SELECT ON assets_aggregation TO app_b3_quotation_histories_webapi;

CREATE USER app_prometheus_postgres_exporter WITH PASSWORD '****';
GRANT CONNECT ON DATABASE postgres TO app_prometheus_postgres_exporter;
GRANT pg_monitor to app_prometheus_postgres_exporter;

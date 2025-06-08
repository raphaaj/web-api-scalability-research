# Estudo Referente à Escalabilidade de APIs Web

* [Projeto da API de histórico de cotações](/applications/B3.QuotationHistories.WebApi/)
* [Projeto para importação do histórico de cotações para a base de dados PostgreSQL](/applications/B3.QuotationHistories.Importer/)
* [Arquivo de configuração para execução da base de dados PostgreSQL](/database/postgresql.conf)
* [Script SQL para definição das estruturas da base de dados PostgreSQL](/database/setup.sql)
* [Arquivo de configuração para execução do Prometheus](/prometheus/prometheus.yml)
* [Arquivo de configuração para execução do Prometheus - modo somente leitura](/prometheus/prometheus-ro.yml)
* [Arquivo docker-compose para deploy dos componentes para realização dos testes](/docker-compose.yml)
* [Arquivo docker-compose para deploy do Prometheus e do Grafana - modo somente leitura](/docker-compose-ro.yml)
* [Script de teste do k6 para aquecimento do pool de conexões da API](/k6/scripts/warmup.js)
* [Script de teste do k6 para identificação da capacidade máxima da API](/k6/scripts/breakpoint.js)

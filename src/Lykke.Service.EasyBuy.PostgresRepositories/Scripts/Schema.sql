-- Tables

CREATE TABLE instruments (
  id uuid NOT NULL,
  asset_pair varchar(12) NOT NULL,
  exchange varchar(50) NOT NULL,
  lifetime time NOT NULL,
  overlap_time time NOT NULL,
  markup numeric(19,10) NOT NULL,
  min_quote_volume numeric(19,10) NOT NULL,
  max_quote_volume numeric(19,10) NOT NULL,
  status smallint NOT NULL
);

CREATE TABLE prices (
  id uuid NOT NULL,
  asset_pair varchar(12) NOT NULL,
  "value" numeric(19,10) NOT NULL,
  base_volume numeric(19,10) NOT NULL,
  quote_volume numeric(19,10) NOT NULL,
  valid_from timestamp with time zone NOT NULL,
  valid_to timestamp with time zone NOT NULL,
  exchange varchar(50) NOT NULL,
  original_value numeric(19,10) NOT NULL,
  markup numeric(19,10) NOT NULL,
  created_date timestamp with time zone NOT NULL
);

CREATE TABLE orders (
  id uuid NOT NULL,
  client_id uuid NOT NULL,
  price_id uuid NOT NULL,
  asset_pair varchar(12) NOT NULL,
  base_volume numeric(19,10) NOT NULL,
  quote_volume numeric(19,10) NOT NULL,
  status smallint NOT NULL,
  error varchar(512) NULL,
  created_date timestamp with time zone NOT NULL
);

CREATE TABLE transfers (
  id uuid NOT NULL,
  order_id uuid NOT NULL,
  type smallint NOT NULL,
  created_date timestamp with time zone NOT NULL
);

-- Views

CREATE VIEW latest_prices
AS
SELECT id, asset_pair, value, base_volume, quote_volume, valid_from, valid_to, exchange, original_value, markup, created_date
FROM (
    	SELECT *, ROW_NUMBER() OVER(PARTITION BY asset_pair ORDER BY valid_to DESC) AS "row_number"
    	FROM prices
	) ordered_prices
WHERE ordered_prices."row_number" = 1;

-- Primary keys

ALTER TABLE ONLY instruments
    ADD CONSTRAINT pk_instruments PRIMARY KEY (id);

ALTER TABLE ONLY prices
    ADD CONSTRAINT pk_prices PRIMARY KEY (id);

ALTER TABLE ONLY orders
    ADD CONSTRAINT pk_orders PRIMARY KEY (id);

ALTER TABLE ONLY transfers
    ADD CONSTRAINT pk_transfers PRIMARY KEY (id);

-- Foreign keys

ALTER TABLE orders
    ADD CONSTRAINT fk_orders_prices FOREIGN KEY (price_id)
    REFERENCES prices (id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;
    
ALTER TABLE transfers
    ADD CONSTRAINT fk_transfers_orders FOREIGN KEY (order_id)
    REFERENCES orders (id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;

-- Unique indices

ALTER TABLE instruments
    ADD CONSTRAINT uq_instruments_asset_pair UNIQUE (asset_pair);
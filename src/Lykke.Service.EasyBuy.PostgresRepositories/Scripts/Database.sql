CREATE DATABASE easy_buy
    WITH 
    OWNER = lykkex
    ENCODING = 'UTF8'
    LC_COLLATE = 'English_United States.1252'
    LC_CTYPE = 'English_United States.1252'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

COMMENT ON DATABASE easy_buy
    IS 'https://github.com/LykkeCity/Lykke.Service.EasyBuy';
#!/bin/bash
set -e

# Create user
psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" <<-EOSQL
    CREATE USER $DB_USERNAME WITH PASSWORD '$DB_PASSWORD';
    CREATE DATABASE $DATABASE_NAME;
    GRANT ALL PRIVILEGES ON DATABASE $DATABASE_NAME TO $DB_USERNAME;
EOSQL

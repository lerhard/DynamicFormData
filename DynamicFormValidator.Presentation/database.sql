CREATE TYPE data_type AS ENUM(
   'STRING','UUID','INT','FLOAT','DOUBLE','DECIMAL','DATE','DATETIME','BOOLEAN' 
);

CREATE TYPE validation_type AS ENUM(
   'REQUIRED',
   'LESS_THAN',
   'EQUALS',
   'GREATER_THAN',
   'LESS_THAN_OR_EQUALS',
   'GREATER_THAN_OR_EQUALS',
   'NOT_EQUALS',
   'REGEX',
    'AGE'
);

CREATE TABLE forms
(
    id        SERIAL PRIMARY KEY,
    parent_id INTEGER,
    name VARCHAR(255) NOT NULL,
    description VARCHAR(255) NOT NULL,
    fields JSONB  NOT NULL,
    database_info JSONB NOT NULL,
    validation_info JSONB NOT NULL,
    created_at TIMESTAMP(3) WITHOUT TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP(3) WITHOUT TIME ZONE
);

INSERT INTO forms(name,description,fields,database_info, validation_info)
VALUES ('Person','Person Insertion Form',
        '[
          {
            "id": 1,
            "name": "id",
            "label": "Person id",
            "type": 2,
            "hidden": true
          },
          {
            "id": 2,
            "name": "name",
            "label": "Person Name",
            "type": 0,
            "hidden": false
          },
          {
            "id": 3,
            "name": "surname",
            "label": "Person Surname",
            "type": 0,
            "hidden": false
          },
          {
            "id": 4,
            "name": "birth_date",
            "label": "Person Birth Date",
            "type": 6,
            "hidden": false
          }
        ]'::jsonb,
        '[
          {
            "id": 1,
            "field_id": 1,
            "table": "persons",
            "column_name": "id",
            "type": 2,
            "is_primary_key": true,
            "ignore_on_insert":true,
            "ignore_on_update":true
          },
          {
            "id": 2,
            "field_id": 2,
            "table": "persons",
            "column_name": "name",
            "type": 0,
            "is_primary_key": false,
            "ignore_on_insert":false,
            "ignore_on_update":true
          },
          {
            "id": 3,
            "field_id": 3,
            "table": "persons",
            "column_name": "surname",
            "type": 0,
            "is_primary_key": false,
            "ignore_on_insert":false,
            "ignore_on_update":true
          },
          {
            "id": 4,
            "field_id": 4,
            "table": "persons",
            "column_name": "birth_date",
            "type": 6,
            "is_primary_key": false,
            "ignore_on_insert":false,
            "ignore_on_update":false
          }
        ]'::jsonb,
        '[
          {
            "id": 1,
            "field_id": 2,
            "validation_type": 0,
            "validation_values": [],
            "error_message": "O campo Nome é obrigatório",
            "ignore_on_insert":false,
            "ignore_on_update":true
          },
          {
            "id": 2,
            "field_id": 3,
            "validation_type": 0,
            "validation_values": [],
            "error_message": "O campo Sobrenome é obrigatório",
            "ignore_on_insert":false,
            "ignore_on_update":true
          },
          {
            "id": 3,
            "field_id": 4,
            "validation_type": 0,
            "validation_values": [],
            "error_message": "Data de Nascimento é obrigatória",
            "ignore_on_insert":false,
            "ignore_on_update":true
          },
          {
            "id": 4,
            "field_id": 4,
            "validation_type": 8,
            "validation_values": [18],
            "error_message": "É obrigatório que a pessoa seja maior de 18 anos",
            "ignore_on_insert":false,
            "ignore_on_update":false
          },
          {
            "id": 5,
            "field_id": 1,
            "validation_type": 0,
            "validation_values": [],
            "error_message": "Informe o id da entidade",
            "ignore_on_insert":true,
            "ignore_on_update":false
          }
        ]'::jsonb);

CREATE TABLE persons
(
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    surname VARCHAR(255) NOT NULL,
    birth_date DATE NOT NULL,
    created_at TIMESTAMP(3) WITHOUT TIME ZONE DEFAULT CURRENT_TIMESTAMP
);
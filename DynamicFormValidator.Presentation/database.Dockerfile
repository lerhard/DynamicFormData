FROM  postgres:latest
ENV POSTGRES_PASSWORD=password123
ENV POSTGRES_DB=postgres
COPY ./database.sql /docker-entrypoint-initdb.d/
EXPOSE 5432


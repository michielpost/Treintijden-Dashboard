FROM microsoft/aspnet:1.0.0-beta2

COPY . /app
WORKDIR /app
RUN ["kpm", "restore"]

WORKDIR /app/Trein.Services
RUN ["kpm", "restore"]

WORKDIR /app/TreintijdenDash
RUN ["kpm", "restore"]

EXPOSE 5004
ENTRYPOINT ["k", "kestrel"]
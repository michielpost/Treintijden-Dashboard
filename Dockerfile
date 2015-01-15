FROM microsoft/aspnet

COPY . /app
WORKDIR /app
RUN ["kpm", "restore"]

WORKDIR /app/Trein.Services
RUN ["kpm", "restore"]

WORKDIR /app/TreintijdenDash
RUN ["kpm", "restore"]

EXPOSE 5004
ENTRYPOINT ["k", "kestrel"]
FROM markvnext/aspnet

ADD ./supervisord.conf /etc/supervisor/conf.d/supervisord.conf

ADD . /app/  
WORKDIR /app
RUN ["kpm", "restore"]

WORKDIR /app/Trein.Services
RUN ["kpm", "restore"]

WORKDIR /app/TreintijdenDash
RUN ["kpm", "restore"]

EXPOSE 5004


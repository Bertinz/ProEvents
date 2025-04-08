FROM node:14.15.5


WORKDIR /app

COPY package*.json ./
COPY ./src ./src
RUN npm install
RUN npm install -g serve
RUN rm -fr node_modules


EXPOSE 3000

CMD [ "serve", "-s" ]

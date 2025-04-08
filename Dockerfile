FROM node:14.15.5

WORKDIR /src

COPY package*.json ./
RUN npm install --only=production

COPY . .

EXPOSE 3000

CMD ["npm", "start"]
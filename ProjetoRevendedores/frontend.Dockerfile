# Build da aplicação com Node
FROM node:20-alpine AS build
WORKDIR /app/frontend

COPY frontend/package*.json ./

RUN npm install

COPY frontend/ ./

RUN npm run build

# Imagem final com Nginx
FROM nginx:1.25-alpine AS production
COPY --from=build /app/frontend/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]

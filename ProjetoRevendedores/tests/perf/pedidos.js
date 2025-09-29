import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
  vus: 5,
  duration: '30s',
  thresholds: {
    http_req_duration: ['p(95)<500']
  }
};

const API_BASE = __ENV.API_BASE_URL ?? 'http://localhost:5000';

export default function () {
  const payload = JSON.stringify({ observacao: 'Carga de teste k6' });
  const response = http.post(`${API_BASE}/api/pedidos`, payload, {
    headers: { 'Content-Type': 'application/json' }
  });

  check(response, {
    'status é 201 ou 200': (res) => res.status === 201 || res.status === 200
  });

  sleep(1);
}

import http from 'k6/http';
import { check, sleep } from 'k6';

//Adaptar para realizar testes de carga na API conforme necessário.
export let options = {
    stages: [
        { duration: '10s', target: 10 },
        { duration: '30s', target: 10 },
        { duration: '10s', target: 0 },
    ],
    thresholds: {
        http_req_duration: ['p(95)<500'],  // 95% das requisições devem levar < 500ms
    },
};

const BASE_URL = 'http://localhost:8081';
const AUTH_URL = 'http://localhost:8082/api/usuarios/login';

export default function () {
    let loginRes = http.post(AUTH_URL, JSON.stringify({
        email: 'admin@email.com',
        senha: '123456'
    }), {
        headers: { 'Content-Type': 'application/json' }
    });

    const token = loginRes.json('token');
    check(loginRes, {
        'login com sucesso': (r) => r.status === 200 && token !== '',
    });

    const authHeaders = {
        headers: {
            Authorization: `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
    };

    let res = http.get(`${BASE_URL}/api/itens`, authHeaders);
    check(res, {
        'GET /api/itens OK': (r) => r.status === 200,
    });

    sleep(1);
}

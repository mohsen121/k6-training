import http from 'k6/http';
import { check, sleep } from 'k6';

export let options = {
    vus: 10, // number of virtual users
    duration: '30s', // test duration
};

export default function () {
    let res = http.get('https://localhost:7094/weatherforecast');
    check(res, {
        'is status 200': (r) => r.status === 200,
        'response body is not empty': (r) => r.body.length > 0,
    });
    sleep(1); // wait for 1 second between iterations
}

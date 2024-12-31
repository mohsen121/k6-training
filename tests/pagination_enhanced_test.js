import http from 'k6/http';
import { check, sleep } from 'k6';

export let options = {
    vus: 10, // Number of virtual users
    duration: '30s', // Duration of the test
};

export default function () {
    // Get the current iteration number
    let page = __ITER + 1; // Start with page 1 (since __ITER starts at 0)

    // URL with pagination
    let url = `https://localhost:7094/dbWeatherforecastEnhanced?page=${page}&count=10`;

    // Make the HTTP GET request
    let response = http.get(url);

    // Validate the response
    check(response, {
        'status is 200': (r) => r.status === 200,
        'response contains items': (r) => JSON.parse(r.body).length > 0,
    });

    // Wait before the next request
    sleep(1); // 1 second pause between iterations
}

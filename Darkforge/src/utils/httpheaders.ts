import { HttpHeaders } from "@angular/common/http";

export function get_headers(): HttpHeaders {
    const token = localStorage.getItem('authToken');

    return token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
}
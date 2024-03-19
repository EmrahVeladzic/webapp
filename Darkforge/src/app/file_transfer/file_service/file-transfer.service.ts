import { Injectable } from '@angular/core';
import { HttpClient,HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
  
})
export class FileTransferService {
   base_url = 'https://localhost:7032/api/';


  constructor(private http:HttpClient) { }


  sendData(endpoint:string, data:JSON){


    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    const url = this.base_url+endpoint;

    return this.http.post(url,data,{headers:headers});

    
  }

}

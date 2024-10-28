import { Injectable } from '@angular/core';
import { HttpClient,HttpHeaders, HttpResponse } from '@angular/common/http';
import { base_url,image_actions } from '../../app.routes';
import { Subject } from 'rxjs';


@Injectable({
  providedIn: 'root'
  
})
export class FileTransferService {
    private reader? :FileReader;
    public img_text? : string;
    private bmpTaskSource = new Subject<void>();
    bmpTaskCompleted$ = this.bmpTaskSource.asObservable();



  constructor(public http:HttpClient) {
    this.reader = new FileReader();
    this.img_text ="";
  
   }


   async process_bmp(file: File): Promise<void> {
   
    this.reader!.readAsDataURL(file);

    this.reader!.onload = ($event: any) => {
      let img_data = this.reader?.result;

      if (img_data !== undefined) {
        this.img_text = img_data!.toString();
      }

   
      bmp_preview_url = URL.createObjectURL(file);

     
      this.bmpTaskSource.next();
    };
  }

  async process_wav(file : File){


  }



}
export let bmp_preview_url :string;
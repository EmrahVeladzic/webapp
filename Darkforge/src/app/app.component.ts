import { Component, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { RendererComponent } from './renderer/renderer.component';
import { ViewChild } from '@angular/core';
import { HostListener } from '@angular/core';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RendererComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Darkforge';
  
  @ViewChild('file_input') input? : ElementRef<HTMLInputElement>;

  upload_click():void{

    if(this.input){
    this.input.nativeElement.click();
    }
    
  }
  
  upload_file($event:any):void{
    const selected:File = $event.target.files[0];
    
    
  }



}

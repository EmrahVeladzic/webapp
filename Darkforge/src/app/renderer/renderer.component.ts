import { AfterViewInit, Component, OnInit } from '@angular/core';
import { WebGLService } from './webgl_service/web-gl.service';

@Component({
  selector: 'app-renderer',
  standalone: true,
  imports: [],
  templateUrl: './renderer.component.html',
  styleUrl: './renderer.component.css'
})
export class RendererComponent implements OnInit, AfterViewInit {
private out!: HTMLCanvasElement;


ngOnInit(): void {
  this.out = document.getElementById('output') as HTMLCanvasElement;
  if(this.out){
  this.webgl.initialise(this.out);
  }
}

ngAfterViewInit(): void {
  
}

constructor(private webgl:WebGLService){

}


}

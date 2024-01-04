import { AfterViewInit, Component, OnInit } from '@angular/core';
import { WebGLService } from './webgl_service/web-gl.service';
import { tick } from '@angular/core/testing';
import { HttpClientModule } from '@angular/common/http';


@Component({
  selector: 'app-renderer',
  standalone: true,
  imports: [HttpClientModule],
  templateUrl: './renderer.component.html',
  styleUrl: './renderer.component.css',
  providers: [WebGLService]
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

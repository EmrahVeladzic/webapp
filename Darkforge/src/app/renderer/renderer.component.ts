import { AfterViewInit, Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { WebGLService } from './webgl_service/web-gl.service';
import { HttpClientModule } from '@angular/common/http';


@Component({
  selector: 'app-renderer',
  standalone: true,
  imports: [HttpClientModule],
  templateUrl: './renderer.component.html',
  styleUrl: './renderer.component.css',
  providers: []
})
export class RendererComponent implements OnInit, AfterViewInit {
@ViewChild('output',{static:false}) out!: ElementRef<HTMLCanvasElement>;

ngOnInit(): void {
 
}

ngAfterViewInit(): void {
  if(this.out){
    this.webgl.initialise(this.out.nativeElement);    
  }
}

constructor(private webgl:WebGLService){

}


}

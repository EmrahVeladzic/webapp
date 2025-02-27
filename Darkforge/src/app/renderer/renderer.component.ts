import { AfterViewInit, Component, OnInit, ViewChild, ElementRef, HostListener } from '@angular/core';
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
private active:number=0;
private beginX:number=0;
private beginY:number=0;
private beginZ: number=0;

ngOnInit(): void {
 
}

ngAfterViewInit(): void {
  if(this.out){
    this.webgl.initialise(this.out.nativeElement);    
  }
}

constructor(private webgl:WebGLService){

}

  @HostListener('document:mousedown', ['$event'])
  onMouseDown($event: MouseEvent): void {    
    if ($event.target === this.out.nativeElement) {
      if($event.button===0){
        this.beginX=$event.clientX;
        this.beginY=$event.clientY;
        this.active=1;
      }
      else if($event.button===1){
        this.beginZ=$event.clientY;
        this.active=2;
      }
     
     
    }
 
  }

  @HostListener('document:mousemove', ['$event'])
  onMouseMove($event: MouseEvent): void {
    if (this.active===1) {
      
      
        let mouseMovementX=0;
        let mouseMovementY=0;

      
        mouseMovementY = this.beginY - $event.clientY;   
        this.beginY = $event.clientY;
      
      
        mouseMovementX = $event.clientX - this.beginX;   
        this.beginX = $event.clientX;
      

      
        this.webgl.H+=((mouseMovementX/window.innerWidth)*4);
        this.webgl.V+=((mouseMovementY/window.innerHeight)*4);
    
   
        this.webgl.H%=(2*Math.PI);
        this.webgl.V%=(2*Math.PI);

      
      

    }

    else if(this.active===2){

      let mouseMovementZ=0;
      mouseMovementZ = this.beginZ - $event.clientY;   
      this.beginZ = $event.clientY;

      this.webgl.D+=mouseMovementZ/25;

      if(this.webgl.D>-5){
        this.webgl.D=-5;
      }
      if(this.webgl.D<-25){
        this.webgl.D=-25;
      }

    }
  }

  @HostListener('document:mouseup')
  onMouseUp(): void {
    this.active = 0;
  }



}

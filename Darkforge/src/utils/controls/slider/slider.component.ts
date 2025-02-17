import { Component, ElementRef, HostListener, ViewChild,Input, forwardRef} from '@angular/core';
import { InputComponent } from '../input/input.component';
import {NG_VALUE_ACCESSOR } from '@angular/forms';
import { CommonModule } from '@angular/common';



@Component({
  selector: 'app-slider',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './slider.component.html',
  styleUrl: './slider.component.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SliderComponent),
      multi: true,
    },
  ],
})
export class SliderComponent extends InputComponent {
  @ViewChild('thumb',{static:false})thumb!:ElementRef;
  @ViewChild('coloured',{static:false})coloured!:ElementRef;


  @Input() override value: number = this.default;
  @Input() override min:number = 0;
  @Input() override max:number = 100;
  @Input() override default: number = 0;
  @Input() vertical:boolean = false;


  private begin:number = 0;
  private sliderDimension:number=0;
  
  

  public override setValue(v:number):void{
    super.setValue(v);
    this.validateInput();
  }
  public override getValue():number {
    return this.value;
  }

 
  protected override validateInput():void{
    
    super.validateInput();

     let dispValue = ((this.value - this.min) / (this.max - this.min)) * 80;

     
     if(this.vertical){
      this.coloured.nativeElement.style.height = `${80-dispValue}%`; 
     }
     else{
      this.coloured.nativeElement.style.width = `${dispValue}%`; 
     }

    
  
        
  }

  @HostListener('document:mousedown', ['$event'])
  onMouseDown($event: MouseEvent): void {    
    if ($event.target === this.thumb.nativeElement) {
      this.sliderDimensionReset();
      this.active = true;
      this.begin = (this.vertical)? $event.clientY : $event.clientX;
      $event.preventDefault();
      this.onTouched();
    }
 
  }

  @HostListener('document:mousemove', ['$event'])
  onMouseMove($event: MouseEvent): void {
    if (this.active) {
      let mouseMovement=0;

      if(this.vertical){
        mouseMovement = this.begin - $event.clientY;   
        this.begin = $event.clientY;
      }
      else{
      mouseMovement = $event.clientX - this.begin;   
      this.begin = $event.clientX;
      }

      const delta = (mouseMovement / this.sliderDimension) * (this.max - this.min);
     
      

       this.value += delta;
      this.validateInput();
    }
  }

  @HostListener('document:mouseup')
  onMouseUp(): void {
    this.active = false;
  }

  public sliderDimensionReset():void{
    
    this.sliderDimension = (this.vertical)? this.el.nativeElement.offsetHeight : this.el.nativeElement.offsetWidth;
  }


  override ngAfterViewInit() {
    super.ngAfterViewInit();
    this.sliderDimensionReset();
    this.validateInput();
  }

  @HostListener('window:resize')
  onResize() {
    this.sliderDimensionReset();
  }

  override writeValue(v: number): void {
    
    if(this.coloured&&this.thumb){
      super.writeValue(v);
    }
    
  }

  

}

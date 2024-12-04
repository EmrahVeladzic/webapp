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


  protected override value: number = 0;

  private begin:number = 0;
  private sliderDimension:number=0;
    
  override min:number = 0;
  override max:number = 100;
  override default: number = 0;

  @Input() vertical:boolean = false;

  public override setValue(v:number):void{
    super.setValue(v);
    this.validateInput();
  }
  public override getValue():number {
    return this.value;
  }

 
  protected override validateInput():void{
    if(this.value<this.min){
      this.value=this.min;
     }
     else if(this.value>this.max){
      this.value=this.max;
     }
     else{
      this.value/=this.step;
      this.value=Math.round(this.value);
      this.value*=this.step;
     }
     let dispValue = ((this.value - this.min) / (this.max - this.min)) * 80;

     
     if(this.vertical){
      this.coloured.nativeElement.style.height = `${80-dispValue}%`; 
     }
     else{
      this.coloured.nativeElement.style.width = `${dispValue}%`; 
     }

      if(this.active){
        this.onChange(this.value);
        this.valueSubject.next(this.value);
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

  
  override ngOnInit(){
    super.ngOnInit();
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
    this.value=v;
    if(this.coloured&&this.thumb){
      this.validateInput();
    }
    
  }

  

}

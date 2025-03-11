import { Component,ElementRef ,forwardRef, Input, AfterViewInit , OnInit, OnDestroy} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { BehaviorSubject, Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-input',
  standalone: true,
  imports: [],
  templateUrl: './input.component.html',
  styleUrl: './input.component.css',
  providers:[
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputComponent),
      multi: true,
    },
  ]
})
export class InputComponent implements ControlValueAccessor, AfterViewInit {
  constructor(protected el: ElementRef) {} 



  @Input()  min:number =0;
  @Input()  max:number =100;
  @Input()  default:number =0;
  @Input()  step:number =1;

  protected value:number = this.default;
  protected past_value:number=this.default;
  protected active:boolean=false;
  

  protected valueSubject = new BehaviorSubject<number>(this.value);
  public valueChanges$ = this.valueSubject.asObservable();


  protected onChange: (value: number) => void = () => {};
  protected onTouched: () => void = () => {};


  public setValue(v:any):void{

    if(!Number.isNaN(v)){

      this.value = Math.round(v);
      this.active=true;
      this.validateInput();
      this.active=false;
      this.value=v;
      this.validateInput();
      this.onTouched();
    
    }
    else{
      this.setValue(this.past_value);
    }
 
  }
  public getValue():number{
    return this.value;
  }

  registerOnChange(fn: (value: number) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }


  writeValue(v: number): void {
    if (!Number.isNaN(v)) {
      this.value = v;      
      this.validateInput();
    }
  }

  protected  validateInput():void{
    
    this.past_value=this.value;
    this.value=Math.min(Math.max(this.value,this.min),this.max);
    
    this.value/=this.step;
    this.value=Math.round(this.value);
    this.value*=this.step;
   

    if(this.active){
      this.onChange(this.value);
      this.valueSubject.next(this.value);
    }

  }

  ngOnInit(){

    this.step=Math.max(1,this.step);
    this.max=Math.max(this.min,this.max);
    this.default=Math.min(Math.max(this.default,this.min),this.max);
    this.value=this.default;

  }

  ngOnDestroy(){
    this.valueSubject.complete();
  }

  ngAfterViewInit(){
    
  }
}

import { Component, ViewChild ,ElementRef, forwardRef, Input} from '@angular/core';
import { InputComponent } from '../input/input.component';
import { FormsModule, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-numeric',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './numeric.component.html',
  styleUrl: './numeric.component.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => NumericComponent),
      multi: true,
    },
  ],
})
export class NumericComponent extends InputComponent {


  @Input() override  min:number =0;
  @Input() override  max:number =100;
  @Input() override default:number =0;
  @Input() override step:number = 1;

  protected raw_value:string="";

  protected override value: number = 0;

 
  override ngOnInit(){
    super.ngOnInit();
    this.raw_value=this.value.toString();
  }


  public override setValue(v:string): void {
    
   super.setValue(Number.parseInt(v));

   this.raw_value=this.value.toString();
   
  }

  public increment_decrement(i_d:number){
    this.value+=i_d;
    this.active=true;
    this.validateInput();
    this.active=false;
    this.raw_value=this.value.toString();

  }

  public override writeValue(v: number): void {
    super.writeValue(v);
    this.raw_value=this.value.toString();

  }

 

}

import { Component ,Input, OnInit, ViewChild, OnDestroy, ElementRef} from '@angular/core';
import { ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { takeUntil,Subject,Subscription, catchError ,of} from 'rxjs';

export function link_slider_numeric(form:FormGroup, slider:string, numeric:string, min:number, max:number, $destr:Subject<void>):void{
    form.get(slider)?.valueChanges.pipe(takeUntil($destr)).subscribe(val=>{

        if(val<min){
            form.get(slider)?.setValue(min,{emitEvent:false});
            form.get(numeric)?.setValue(min,{emitEvent:false});
        }
        else if(val>max){
            form.get(slider)?.setValue(max,{emitEvent:false});
            form.get(numeric)?.setValue(max,{emitEvent:false});
        }
        else{
            form.get(numeric)?.setValue(val,{emitEvent:false});
        }
    });
      form.get(numeric)?.valueChanges.pipe(takeUntil($destr)).subscribe(val=>{

        if(val<min){
            form.get(slider)?.setValue(min,{emitEvent:false});
            form.get(numeric)?.setValue(min,{emitEvent:false});
        }
        else if(val>max){
            form.get(slider)?.setValue(max,{emitEvent:false});
            form.get(numeric)?.setValue(max,{emitEvent:false});
        }
        else{
            form.get(slider)?.setValue(val,{emitEvent:false});
        }
    });   
}
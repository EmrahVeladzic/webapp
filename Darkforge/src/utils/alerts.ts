import { TranslateService } from "@ngx-translate/core";
export function alert_loclized(t:TranslateService,key:string):void{
    t.get(key).subscribe(r=>{

        alert(r);

    });
}
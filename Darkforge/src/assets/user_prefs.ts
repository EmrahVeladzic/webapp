import { Subject } from "rxjs";

let pref$:Subject<void> =new Subject<void>();

class UserPreferences{


public userId:number;
public language	:string;
public shareAssetOwnership :boolean;


constructor(i:number=0,share:boolean=false, lang:string="en") {
   this.userId=i;
   this.shareAssetOwnership=share;
   this.language=lang;    
}


}

let user_prefs:UserPreferences=new UserPreferences();


function set_prefs(u:UserPreferences):void{
    user_prefs=u;
}

function emit_prefs_change():void{
    pref$.next();
}

export{UserPreferences,user_prefs,set_prefs, pref$,emit_prefs_change}
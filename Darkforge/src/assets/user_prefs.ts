class UserPreferences{

public userId:number;
public language	:string;
public fileLifespan	:number;
public shareAssetOwnership :boolean;


constructor(i:number=0, life:number=0,share:boolean=false, lang:string="en") {
   this.userId=i;
   this.fileLifespan=life;
   this.shareAssetOwnership=share;
   this.language=lang;    
}


}

let user_prefs:UserPreferences=new UserPreferences();


function set_prefs(u:UserPreferences):void{
    user_prefs=u;
}


export{UserPreferences,user_prefs,set_prefs}

const SaveToken=(userData)=>{
    localStorage.setItem("userData",JSON.stringify(userData));
}

const GetUserData=()=>{
    const data=localStorage.getItem("userData");
    if (!data){
        return null;
    }
    return  JSON.parse(data) ;
    }
export {SaveToken,GetUserData};


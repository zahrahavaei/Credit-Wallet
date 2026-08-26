
const SaveToken=(token,userData)=>{
    localStorage.setItem("token",token);
    localStorage.setItem("userData",userData);
}
const GetToken=()=>{
    const token=localStorage.getItem("token");
    return token;
}
const GetUserData=()=>{
    const data=localStorage.getItem("userData");
    if (!data){
        return null;
    }
    return data;
    }
export {SaveToken,GetToken,GetUserData};


import { useState ,useContext} from "react"
import {useNavigate} from "react-router-dom"
import { AuthContext } from "../Context/AuthContext";

const Login=()=>{
   
    const navigate=useNavigate();
    const{userData,setUserData}=useContext(AuthContext);
    const[userName,setUserName]=useState();
    const[password,setPassword]=useState();
    const[message,setMessage]=useState();
    

    const LoginForm=async(e)=>{
         e.preventDefault();
        try{
            const rsp=await fetch('https://localhost:7295/api/user/login',{
                method:"Post",
                headers:{
                    "content-type":"application/json",
                },
                credentials:"include",
                body:JSON.stringify
                ({
                    userName:userName,
                    Password:password
                })
            });
            if (rsp.ok){
                const result=await rsp.json();
                const userDataInfo={
                    userName:result.userName,
                    userRole:result.userRole,
                    firstName:result.firstName,
                    lastName:result.lastName
                }
                 setMessage(result.message);
                console.log("userData",userData);
                setUserData(userDataInfo);
                if(userDataInfo.userRole=="Admin")
                {
                    navigate("/adminDashboard");
                }
                else if(userData.userRole=="Customer")
                {
                    navigate("/customerDashboard");
                }
                else {
                    console.log("login failed");
                }
            }else{
                const result=await  rsp.json();
                console.log(result);
                console.log(result.message);
                setMessage(result.message);
            }
        }catch(error){
            console.log("Error during login",error);
        }
    }
    return(
        <>
        <form  onSubmit={LoginForm}>
           <div>
            <div id="columnLeft">
              <input 
              placeholder="Enter UserName:" 
              onChange={e=>setUserName(e.target.value)}>
              </input>
            </div>
            <div id="columnLeft">
              <input
              placeholder="Enter Password:"
              onChange={e=>setPassword(e.target.value)}>
              </input>
            </div>
            <div id="columnLeft">
               <button 
                type="submit">
                LogIn
               </button>
            </div>
            {message && (
                <div>
                   {message}
                </div>
            )}
            <div id="columnRight">
              
            </div>
           </div>
        </form>
        </>
    )

}
export default Login;
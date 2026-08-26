import { useState ,useContext} from "react"
import {useNavigate} from "react-router-dom"
import {SaveToken}  from "../Services/TokenServices"

const Login=()=>{
   
    const navigate=useNavigate();
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
                body:JSON.stringify
                ({
                    userName:userName,
                    Password:password
                })
            });
            if (rsp.ok){
                const result=await rsp.json();
                const token=result.token;
                const userData={
                    userName:result.userName,
                    userRole:result.userRole,
                    firstName:result.firstName,
                    lastName:result.lastName
                }
                console.log("token",token);
                console.log("userData",userData);
                SaveToken({token,userData});
                if(userData.userRole=="Admin")
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
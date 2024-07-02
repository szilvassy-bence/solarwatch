import {useNavigate} from "react-router-dom";
import "./Register.css";
import { useState } from "react";

export default function Register(){

    const navigate = useNavigate();
    const [registrationData, setRegistrationData] = useState({
        username: "",
        email: "",
        password: ""
    });

    const handleRegister = async (e) => {
        e.preventDefault();
        try {
            await fetch("/api/Auth/Register", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(registrationData),
            });
            navigate("/login");
        } catch(e){
            console.error(e);
        }
    }

    return (
        <div id="register">
            <h1>Register</h1>
            <form onSubmit={handleRegister}>
                <label>
                    <span>User name</span>
                    <input 
                        type="text" 
                        name="username" 
                        placeholder="User name"
                        onChange={(e) => setRegistrationData({...registrationData, username: e.target.value})}/>
                </label>
                <label>
                    <span>Email</span>
                    <input 
                        type="email" 
                        name="email" 
                        placeholder="Email"
                        onChange={(e) => setRegistrationData({...registrationData, email: e.target.value})}/>
                </label>
                <label>
                    <span>Password</span>
                    <input 
                        type="password" 
                        name="password" 
                        placeholder="Password"
                        onChange={(e) => setRegistrationData({...registrationData, password: e.target.value})}/>
                </label>
                <button type="submit">Register</button>
            </form>
        </div>
    )
}
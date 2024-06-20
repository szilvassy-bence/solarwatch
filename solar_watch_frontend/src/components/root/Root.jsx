import {useState, createContext, useContext, useCallback, useRef, useEffect, useMemo} from "react";
import "./Root.css";
import {useNavigate, Outlet} from "react-router-dom";
import Header from "../header";
import {useLocalStorage} from "../../hooks/useLocalStorage.jsx";

export const AuthContext = createContext(null);

export default function Root() {

    const [user, setUser] = useLocalStorage("user", null);
    const navigate = useNavigate();

    const login = async (data) => {
        try {
            let res = await fetch("/api/Auth/Login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(data),
            });
            if (res.status === 400){
                console.error("Bad request. Please check your input.");
                return false;
            }
            let user = await res.json();
            console.log(user);
            setUser(user);
            navigate("/");
            return true;
        } catch (e) {
            console.log(e)
        }
    }

    const logout = () => {
        setUser(null);
        navigate("/", {replace: true});
    }
    
    const value = useMemo(
        () => ({
            user, login, logout
        }), [user]
    )

    return (
        <AuthContext.Provider value={value}>
            <Header/>
            <div id="detail">
                <Outlet/>
            </div>
        </AuthContext.Provider>
    )
}

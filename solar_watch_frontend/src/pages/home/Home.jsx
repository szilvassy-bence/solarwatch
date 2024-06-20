import {useContext, useEffect} from "react";
import {Link} from "react-router-dom";
import { AuthContext } from "../../components/root/Root.jsx"
import "./Home.css"

export default function Home() {
    
    const value = useContext(AuthContext);
    console.log(value);
        
    return (
        <div id="home">
            { !value || value.user == null  ? (
                <>
                    <h1>One more step..</h1>
                    <p>
                        Currently you are not logged in, <br/>
                        <Link to="/login">Login</Link> or <Link to="/register">Register</Link> to use our app!
                    </p>
                </>
            ) : (
                <>
                    <h1>Welcome, {value.user.userName}</h1>
                    <p>
                    Browse our site,<br/>
                    <Link to="/cities">Cities</Link> or <Link to="/profile">Profile</Link> are in your service!
                    </p>
                </>
            )}
        </div>
    )
}
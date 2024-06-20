import "./Login.css"
import {useState, useContext} from "react";
import {AuthContext} from "../../components/root/Root.jsx"


export default function Login() {

    const [email, setEmail] = useState(null);
    const [password, setPassword] = useState(null);
    const [isLoginSuccessful, setIsLoginSuccessful] = useState(null);
    const {login} = useContext(AuthContext);
    
    async function handleLogin(e) {
        e.preventDefault();
        const userToLogin = {
            email: email,
            password: password
        };
        setIsLoginSuccessful(await login(userToLogin));
    }

    return (
        <div id="login">
            <h1>Login</h1>
            <div>
                <form onSubmit={handleLogin}>
                    <label>
                        <span>Email</span>
                        <input type="text" name="email" placeholder="Email" onChange={(e) => setEmail(e.target.value)}/>
                    </label>
                    <label>
                        <span>Password</span>
                        <input type="password" name="password" placeholder="Password"
                               onChange={(e) => setPassword(e.target.value)}/>
                    </label>
                    <button type="submit">Login</button>
                </form>
                { isLoginSuccessful === false && (
                    <div id="invalid-login">
                        <p>Login is not successful, try again!</p>
                    </div>
                )}
            </div>
        </div>
    )
}
import React from 'react'
import ReactDOM from 'react-dom/client'
import {createBrowserRouter, RouterProvider} from 'react-router-dom';
import './index.css'
import Root from "./components/root"
import ErrorPage from "./pages/error"
import Cities, {loader as citiesLoader} from "./pages/cities"
import Login/*, {action as loginAction}*/ from "./pages/login"
import Register from "./pages/register"
import Home from "./pages/home";
import Profile from "./pages/profile";
import City, { loader as cityLoader } from "./pages/city";
import Search, {loader as searchLoader} from './pages/Search';

const router = createBrowserRouter([
    {
        path: "/",
        element: <Root/>,
        errorElement: <ErrorPage/>,
        
        children: [
            {index: true, element: <Home/>},
            {
                path: "/login",
                element: <Login/>,
                //action: loginAction,
            },
            {
                path: "/register",
                element: <Register/>,
            },
            {
                path: "/cities",
                element: <Cities/>,
                loader: citiesLoader
            },
            {
                path: "/cities/:name",
                element: <City/>,
                loader: ({params}) => {
                    return cityLoader(params.name)
                }
            },
            {
                path: "/profile",
                element: <Profile/>
            },
            {
                path: "/search/:searchTerm",
                element: <Search/>,
                loader: ({params}) => {
                    return searchLoader(params.searchTerm)
                }
            }
        ]

    }
])

ReactDOM.createRoot(document.getElementById('root')).render(
    <React.StrictMode>
        <RouterProvider router={router}/>
    </React.StrictMode>,
)

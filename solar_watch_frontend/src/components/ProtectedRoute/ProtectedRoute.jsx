import React from 'react'
import { useContext } from 'react'
import { AuthContext } from '../root/Root'
import { Navigate } from "react-router-dom"

const ProtectedRoute = ({children}) => {

	const {user} = useContext(AuthContext);


	return user ? children : <Navigate to="/login"/>
}

export default ProtectedRoute
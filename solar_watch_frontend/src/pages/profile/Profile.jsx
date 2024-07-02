import "./Profile.css";
import { useEffect, useState, useContext } from "react";
import {AuthContext, FavoriteContext} from "../../components/root/Root";
import CityCard from "../../components/CityCard";

export default function Profile(){

    const {user, logout} = useContext(AuthContext);
    const [favorites, setFavorites] = useContext(FavoriteContext);

    return (
        <div>
            <h1>Profile page</h1>
            <div>
                <h2>Favorite countries</h2>
                { favorites && 
                    <div id="favorites">
                        {favorites.map(fav => (
                            <CityCard key={fav.id} city={fav}/>
                        ))}
                    </div> 
                }
            </div>
        </div>
    )
}
import "./CityCard.css";

import React from "react";
import { useNavigate } from "react-router-dom";
import LikeButton from "../like-button";
import DislikeButton from "../dislike-button";
import { useContext } from "react";
import { AuthContext, FavoriteContext } from "../root/Root";


const CityCard = ({city}) => {

  const {user} = useContext(AuthContext);
  const [favorites, setFavorites] = useContext(FavoriteContext);
	const navigate = useNavigate();
	const cardClick = (e) => {
		let city = e.target.closest(".card").getAttribute("value");
		navigate(`/cities/${city}`);
	}

  return (
    <div key={city.id} value={city.name} className="card" >
      <div className="card-body">
        <h3 onClick={cardClick}>{city.name}</h3>
        <ul>
          <li>Country: {city.country}</li>
          {city.state && <li>State: {city.state}</li>}
        </ul>
      </div>
      { user &&
        <div className="card-footer">
          { favorites.some(c => c.id === city.id) ? 
            <DislikeButton city={city} token={user.token} favorites={favorites} setFavorites={setFavorites}/> :
            <LikeButton city={city} token={user.token} favorites={favorites} setFavorites={setFavorites}/> 
          }
        </div>
      }
    </div>
  );
}

export default CityCard;

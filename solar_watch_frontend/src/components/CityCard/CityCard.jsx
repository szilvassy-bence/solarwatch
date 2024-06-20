import "./CityCard.css";

import React from "react";
import { useNavigate } from "react-router-dom";


const CityCard = ({city}) => {

	const navigate = useNavigate();
	const cardClick = (e) => {
		let city = e.target.closest(".card").getAttribute("value");
		navigate(`/cities/${city}`);
	}

	return (
    <div key={city.id} value={city.name} className="card" onClick={cardClick}>
      <h3>{city.name}</h3>
      <ul>
        <li>Country: {city.country}</li>
        {city.state && <li>State: {city.state}</li>}
      </ul>
    </div>
  );
}

export default CityCard;

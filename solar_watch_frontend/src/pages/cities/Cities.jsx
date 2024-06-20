import {useLoaderData, useNavigate} from "react-router-dom";
import "./Cities.css"
import "../../components/CityCard";
import CityCard from "../../components/CityCard";

export default function Cities() {
    const cities = useLoaderData();
    const navigate = useNavigate();
    
    const cardClick = (e) => {
        let city = e.target.closest(".card").getAttribute("value");
        navigate(`/cities/${city}`);
    }
    
    return(
        <div>
            <h1>Cities</h1>
            <div id="card-container">
                {cities.map(x => (
                    <CityCard key={x.id} city={x} />
                ))}
            </div>
        </div>
    )
}
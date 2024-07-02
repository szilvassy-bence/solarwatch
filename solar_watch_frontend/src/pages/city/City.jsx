import "./City.css";
import { useState } from "react";
import { useLoaderData, useNavigate } from "react-router-dom";
import * as service from "./service";

export default function City() {
  const loader = useLoaderData();
  const [sunriseSunsets, setSunriseSunsets] = useState(() => {
    if (loader.hasOwnProperty("city")) {
      console.log(loader);
      return loader.city.sunriseSunsets;
    } else {
      return [];
    }
  });
  const [sunInfoExist, setSunInfoExist] = useState(false);
  const [showConfirmDelete, setShowConfirmDelete] = useState(false);
  const [edit, setEdit] = useState(false);
  const [newSunInfo, setNewSunInfo] = useState(false);
  const [date, setDate] = useState(service.getTodayDate());
  const navigate = useNavigate();

  const deleteCity = () => {
    if (edit) {
        setEdit(false);
    }
    setShowConfirmDelete(true);
  };

  const confirmDeleteCity = async () => {
    try {
      const res = await fetch(`/api/SolarWatch/cities/${loader.city.id}/delete`, {
        method: "DELETE",
      });
      if (res.status === 204) {
        navigate("/cities");
      }
    } catch (e) {
      console.log(e);
    }
  };

  const cancelDeleteCity = () => {
    setShowConfirmDelete(false);
  };

  const editCity = () => {
    setEdit((prevState) => !prevState);
  };

  const deleteSunInfo = async (e) => {
    try {
      const res = await fetch(
        `/api/SolarWatch/sunrisesunsets/${e.target.closest("svg").id}/Delete`,
        {
          method: "DELETE",
        }
      );
      if (res.status === 204) {
        const filtered = sunriseSunsets.filter(x => x.id !== Number(e.target.closest("svg").id));
        setSunriseSunsets(filtered);
    }
    } catch (error) {
      console.log(error);
    }
  };

  const submitNewDate = async (e) => {
    e.preventDefault();
    if (sunriseSunsets && sunriseSunsets.some(sunInfo => sunInfo.day === date)){
        setSunInfoExist(true);
        return;
    }
    try {
        const res = await fetch(`/api/SolarWatch/sunrisesunsets/${loader.city.name}/${date}`);
        if (res.status === 200) {
            const sunInfo = await res.json();
            if (!sunriseSunsets){
              setSunriseSunsets([sunInfo]);
            } else{
              setSunriseSunsets(prevSunriseSunsets => [...prevSunriseSunsets, sunInfo]);
            }
            setDate(service.getTodayDate());
            setSunInfoExist(false);
            setNewSunInfo(false);
        }
    } catch (error) {
        console.log(error);
    }
  };

  const addCity = () => {
    if (edit) {
        setEdit(false);
    }
    setNewSunInfo(true);
  }

  return (
    <>
      {  loader.hasOwnProperty("status") && loader.status === 200 ? (
        <>
          <div id="city-nav">
            <button onClick={addCity}>New day</button>
            <button onClick={editCity} className={edit ? "active" : ""}>
              Edit
            </button>
            <button onClick={deleteCity}>Delete</button>
          </div>
          <div id="city-content">
            <div id="city-detail">
              <h1>{loader.city.name}</h1>
              <h3>{loader.city.country}</h3>
              <h3>{loader.city.state}</h3>
            </div>
            <div id="sun-detail-wrapper">
              <h2>Sun information</h2>
              {sunriseSunsets && sunriseSunsets.length > 0 ? (
                sunriseSunsets.map((x) => (
                  <div key={x.id} className="sun-detail-div">
                    <h3>{x.day}</h3>
                    <p>{x.sunrise}</p>
                    <p>{x.sunset}</p>
                    {edit && (
                      <svg
                        id={x.id}
                        onClick={deleteSunInfo}
                        className="svg-suninfo-delete w-6 h-6 text-gray-800 dark:text-white"
                        aria-hidden="true"
                        xmlns="http://www.w3.org/2000/svg"
                        width="24"
                        height="24"
                        fill="currentColor"
                        viewBox="0 0 24 24"
                      >
                        <path
                          fillRule="evenodd"
                          d="M2 12C2 6.477 6.477 2 12 2s10 4.477 10 10-4.477 10-10 10S2 17.523 2 12Zm7.707-3.707a1 1 0 0 0-1.414 1.414L10.586 12l-2.293 2.293a1 1 0 1 0 1.414 1.414L12 13.414l2.293 2.293a1 1 0 0 0 1.414-1.414L13.414 12l2.293-2.293a1 1 0 0 0-1.414-1.414L12 10.586 9.707 8.293Z"
                          clipRule="evenodd"
                        />
                      </svg>
                    )}
                  </div>
                ))
              ) : (
                <div>
                  <h3>No sun information</h3>
                </div>
              )}
            </div>
          </div>
          {showConfirmDelete && (
            <div className="pop-up-wrapper">
              <div className="pop-up-content">
                <svg
                  onClick={cancelDeleteCity}
                  className="w-6 h-6 text-gray-800 dark:text-white"
                  aria-hidden="true"
                  xmlns="http://www.w3.org/2000/svg"
                  width="24"
                  height="24"
                  fill="currentColor"
                  viewBox="0 0 24 24"
                >
                  <path
                    fillRule="evenodd"
                    d="M2 12C2 6.477 6.477 2 12 2s10 4.477 10 10-4.477 10-10 10S2 17.523 2 12Zm7.707-3.707a1 1 0 0 0-1.414 1.414L10.586 12l-2.293 2.293a1 1 0 1 0 1.414 1.414L12 13.414l2.293 2.293a1 1 0 0 0 1.414-1.414L13.414 12l2.293-2.293a1 1 0 0 0-1.414-1.414L12 10.586 9.707 8.293Z"
                    clipRule="evenodd"
                  />
                </svg>
                <h2>Do you really want to delete this city?</h2>
                <div id="confirm-delete-button-con">
                  <button onClick={confirmDeleteCity}>Delete</button>
                  <button onClick={cancelDeleteCity}>Cancel</button>
                </div>
              </div>
            </div>
          )}
          {newSunInfo && (
            <div className="pop-up-wrapper">
              <div className="pop-up-content">
                <svg
                  onClick={() => setNewSunInfo(false)}
                  className="w-6 h-6 text-gray-800 dark:text-white"
                  aria-hidden="true"
                  xmlns="http://www.w3.org/2000/svg"
                  width="24"
                  height="24"
                  fill="currentColor"
                  viewBox="0 0 24 24"
                >
                  <path
                    fillRule="evenodd"
                    d="M2 12C2 6.477 6.477 2 12 2s10 4.477 10 10-4.477 10-10 10S2 17.523 2 12Zm7.707-3.707a1 1 0 0 0-1.414 1.414L10.586 12l-2.293 2.293a1 1 0 1 0 1.414 1.414L12 13.414l2.293 2.293a1 1 0 0 0 1.414-1.414L13.414 12l2.293-2.293a1 1 0 0 0-1.414-1.414L12 10.586 9.707 8.293Z"
                    clipRule="evenodd"
                  />
                </svg>
                <h2>Add a new suninfo by date!</h2>
                { sunInfoExist && (
                    <h3>Sun info is already in the list, try a new date!</h3>
                ) }
                <div>
                  <form onSubmit={submitNewDate}>
                    <input type="date" onChange={(e) => setDate(e.target.value)} />
                    <button>Submit</button>
                  </form>
                </div>
              </div>
            </div>
          )}
        </> ) : loader.status === 404 ? (
          <div>
            <h1>Object not found.</h1>
          </div>
        ) : (
          <div>
            <h1>Unauthorized</h1>
            <h2>Only registered users can access this content</h2>
          </div>
        )
      } 
    </>
      
  );
}

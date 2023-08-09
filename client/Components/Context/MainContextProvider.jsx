import React, { createContext, useState } from 'react';

export const MainContext = createContext();

function MainContextProvider({ children }) {
  // const { sendPushNotification } = useNotificationContext();
  const [authenticated, setAuthenticated] = useState(false);
  const [userName, setUserName] = useState('');
  const [userId, setUserId] = useState('');
  const [userEmail, setUserEmail] = useState('');
  const [isLoading, setIsLoading] = useState(true);
  const [pendingTaskList, setPendingTaskList] = useState([]);

  // HomeScreen method
  const capitalizeFirstLetter = (str) => {
    return str.charAt(0).toUpperCase() + str.slice(1);
  };

  // ScheduleScreen method
  const convertDateFormat = (dateString) => {
    const [year, month, day] = dateString?.split('/');
    return `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`;
  };

  // Separate the saveUserImage function
  const saveUserImage = async () => {
    try {
      if (userImage) {
        await AsyncStorage.setItem('userImage', userImage);
        console.log('user image saved successfully');
      } else {
        await AsyncStorage.removeItem('userImage');
      }
    } catch (error) {
      console.log('Error saving user image', error);
    }
  };

  const loadPendingTask = async (userId) => {
    try {
      const response = await fetch(`${Server_path2}/api/tasks/PendingTasksByUserId/${userId}`);
      if (response.ok) {
        const data = await response.json();
        setPendingTaskList(data);
      } else {
        console.log('Got to else in Pending tasks');
        throw new Error('Request failed');
      }
    } catch (error) {
      console.error('Error loading tasks:', error);
    }
  };

  const MainContextValues = {
    isLoading,
    setIsLoading,
    setAuthenticated,
    authenticated,
    setUserName,
    userName,
    userId,
    setUserId,
    userEmail,
    setUserEmail,
    capitalizeFirstLetter,
    convertDateFormat,
    pendingTaskList,
    setPendingTaskList,
    userEmail
  };

  return (
    <MainContext.Provider value={MainContextValues}>
      {children}
    </MainContext.Provider>
  );
}

export default MainContextProvider;

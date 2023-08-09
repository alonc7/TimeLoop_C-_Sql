import React from 'react';
import MainContextProvider from "./Components/Context/MainContextProvider";
import Main from "./Components/Main";
import { Provider as PaperProvider, DefaultTheme } from 'react-native-paper';
import { SafeAreaProvider } from 'react-native-safe-area-context';
// import { NotificationProvider } from './Components/Context/NotificationContext';

export default function App() {

  return (
    <SafeAreaProvider>
      <MainContextProvider>
        {/* <NotificationProvider> */}
          <PaperProvider theme={DefaultTheme}>
            <Main />
          </PaperProvider>
        {/* </NotificationProvider> */}
      </MainContextProvider>
    </SafeAreaProvider >

  );
}


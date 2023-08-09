import React, { useEffect, useState, useContext } from 'react';
import { View, StyleSheet, FlatList } from 'react-native';
import { LinearGradient } from 'expo-linear-gradient';
import { FloatingAction } from "react-native-floating-action";
import { AntDesign } from '@expo/vector-icons';
import TaskInput from '../Components/TaskInput';
import GoalItem from '../Components/GoalItem';
import COLORS from '../constants/colors';
import { MainContext } from '../Components/Context/MainContextProvider';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { Server_path2 } from '../utils/api-url';

const TasksScreen = () => {
  const [modalIsVisible, setModalIsVisible] = useState(false); // boolean for visualise of the modal ( is it visual right now?)
  const [isHidden, setIsHidden] = useState(true); // boolean for setting the modal hidden or not. 
  const [taskList, setTaskList] = useState([]); // array of tasks 
  const { setUserId, userId, userEmail } = useContext(MainContext);


  useEffect(() => {
    // retrieveUserId();
    const loadTask = async (userId) => {
      try {
        const response = await fetch(`${Server_path2}/api/tasks/PendingTasksByUserId/${userId}`);
        if (response.ok) {
          const data = await response.json(); //data returned from fetch result
          const sortedTasks = await sortTasksByPriority(data); // sorting the data according to priority
          setTaskList(sortedTasks); // setting the taskList as sorted data. 

        }
        throw new Error('Request failed');

      } catch (error) {
        // const message = `An error has occured in TaskScreen: ${error.message}`;
        // console.log(message);
      }
    }
    loadTask(userId);
  }, [taskList]);

  function sortTasksByPriority(tasks) {
    return tasks.sort(comparePriority);
  }
  function comparePriority(a, b) {
    const priorityOrder = { high: 1, medium: 2, low: 3 };  // Define priority order using an object
    return priorityOrder[a.Priority] - priorityOrder[b.Priority];  // Compare the priority values of tasks 'a' and 'b'
  }


  function toggleBtn() {
    setIsHidden(!isHidden);
  }

  function handleModalIsVisible() {
    setModalIsVisible(!modalIsVisible);
  }


  const addTaskHandler = async (title, startDate, dueDate, startTime, dueTime, priority) => {
    const taskData = {
      task: {
        Title: title,
        StartDate: startDate,
        DueDate: dueDate,
        StartTime: startTime,
        DueTime: dueTime,
        Priority: priority,
        Status: "pending",
      },
      userEmail: userEmail
    };
    try {
      console.log(title, startDate, dueDate, startTime, dueTime, priority);
      const response = await fetch(`${Server_path2}/api/Tasks/AddTask`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(taskData)
      });

      if (response.ok) {
        console.log('Task created successfully');
      } else {
        console.log('Failed to create task:', response.status);
      }
    } catch (error) {
      console.error('Error creating task:', error);
    }
  };


  // function deleteAllTasks(key) {
  //   setTaskList([]);
  //   toggleBtn();
  // }


  const actions = [
    {
      text: 'Add Task',
      icon: <AntDesign name="plus" size={20} color="white" />,
      name: 'add_task',
      position: 2,
    },
  ];
  return (
    <LinearGradient
      style={styles.container}
      colors={[COLORS.secondary, COLORS.primary]}
    >
      <View style={styles.container}>
        <TaskInput
          visible={modalIsVisible}
          onAddTask={addTaskHandler}
          onClose={handleModalIsVisible}
          toggleBtn={toggleBtn}
          setTasks={setTaskList}
        />
        <View style={styles.tasksContainer}>
          <FlatList
            contentContainerStyle={{ justifyContent: 'center' }}
            data={taskList}
            renderItem={({ item }) => (
              <GoalItem
                text={item.Title}
                startDate={item.StartDate}
                endDate={item.DueDate}
                id={item.Id}
                startHour={item.StartTime}
                endHour={item.DueTime}
                priority={item.Priority}
              />
            )}
            keyExtractor={(item) => item.Id.toString()} // Use the 'Id' property as the key
            alwaysBounceVertical={false}
          />


          {/* <Pressable
            style={[styles.buttonText, isHidden && styles.btnHide]}
            onPress={deleteAllTasks}
          >
            <Text style={styles.buttonText}>Delete All Tasks</Text>
          </Pressable> */}
          <FloatingAction
            color='#222222'
            position={'left'}
            buttonSize={30}
            overrideWithAction={true}
            showBackground={false}
            actions={actions}
            onPressItem={handleModalIsVisible}


          />
        </View>
      </View>
    </LinearGradient>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'flex-end',
  },


  tasksContainer: {
    flex: 5
  },
  buttonText: {
    flexDirection: 'row',
    color: '#B2A4FF',
    fontSize: 18,
    textDecorationLine: 'underline',
    fontWeight: 'bold',
    marginBottom: 40,
    justifyContent: 'center',
  },
  btnHide: {
    display: 'none'
  }
});

export default TasksScreen;

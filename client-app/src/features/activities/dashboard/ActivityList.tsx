import React from 'react';
import { Button, Item, Label, Segment } from 'semantic-ui-react';
import { Activity } from '../../../app/models/activity';

interface Probs{
    activities: Activity[];
}

export default function ActivityList({activities}:Probs) {
    return(
        <Segment>
            <Item.Group divided>
                {activities.map(activities=> (
                    <Item key={activities.id}>
                    <Item.Content>
                        <Item.Header as='a'>{activities.title}</Item.Header>
                        <Item.Meta>{activities.date}</Item.Meta>
                        <Item.Description>
                            <div>{activities.description}</div>
                            <div>{activities.city}, {activities.venue}</div>
                        </Item.Description>
                        <Item.Extra>
                            <Button floated='right' content='View' color='blue' />
                            <Label basic content={activities.category} />
                        </Item.Extra>
                    </Item.Content>
                    </Item>
                ))}
            </Item.Group>
        </Segment>

    )
}
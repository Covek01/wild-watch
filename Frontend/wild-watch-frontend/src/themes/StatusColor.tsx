

export class StatusColor{
    extinctInWild: string = '#6e43a3'
    endangered: string = '#b71c1c'
    protected: string = '#3378af'
    leastConcern: string = '#234915'

}

export class ExtinctInWild extends StatusColor{
    value: string = '#6e43a3'
}

export class Endangered extends StatusColor{
    value: string = '#b71c1c'
}

export class Protected extends StatusColor{
    value: string = '#3378af'
}

export class LeastConcern extends StatusColor{
    value: string = '#234915'
}

export class StatusFactory{
    static createStatus(status: string){
        if (status === 'Extinct In Wild'){
            return new ExtinctInWild()
        }
        else if (status === 'Endangered'){
            return new Endangered()
        }
        else if (status === 'Protected'){
            return new Protected()
        }
        else {
            return new LeastConcern()
        }
    }
}
